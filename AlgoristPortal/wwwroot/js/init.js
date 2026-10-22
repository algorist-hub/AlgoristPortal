(() => {
  if (window.__siteInitialized) return;
  window.__siteInitialized = true;

  const body = document.body;
  const $ = (s) => document.querySelector(s);

  // ---- AOS ----
  window.AOS?.init({ duration: 600, easing: "ease-in-out", once: true, mirror: false });

  // ---- Estado de rolagem (consulta o DOM a cada chamada: sobrevive à navegação aprimorada) ----
  function updatePageState() {
    const header = $("#header");
    if (header && (header.classList.contains("scroll-up-sticky") ||
                   header.classList.contains("sticky-top") ||
                   header.classList.contains("fixed-top"))) {
      body.classList.toggle("scrolled", window.scrollY > 100);
    }
    $("#scroll-top")?.classList.toggle("active", window.scrollY > 100);

    const navLinks = document.querySelectorAll(".navmenu a");
    const position = window.scrollY + 200;
    let activeLink = null;

    navLinks.forEach((link) => {
      if (!link.hash) return;
      const section = document.querySelector(link.hash);
      if (section && position >= section.offsetTop &&
          position <= section.offsetTop + section.offsetHeight) {
        activeLink = link;
      }
    });

    if (window.scrollY + window.innerHeight >= document.documentElement.scrollHeight - 1 &&
        navLinks.length > 0) {
      activeLink = navLinks[navLinks.length - 1];
    }
    navLinks.forEach((link) => link.classList.toggle("active", link === activeLink));
  }

  function scrollToHash(hash) {
    if (!hash) {
      window.scrollTo({ top: 0, behavior: "smooth" });
      return;
    }
    const section = document.getElementById(decodeURIComponent(hash.slice(1)));
    if (!section) return;
    const margin = Number.parseInt(getComputedStyle(section).scrollMarginTop, 10) || 0;
    window.scrollTo({ top: section.offsetTop - margin, behavior: "smooth" });
  }

  // ---- Cliques (delegados, na fase de captura, antes do Blazor interceptar o link) ----
  document.addEventListener("click", (e) => {
    // Menu mobile (ajuste o seletor ao do seu Header)
    const toggle = e.target.closest(".mobile-nav-toggle");
    if (toggle) {
      const active = body.classList.toggle("mobile-nav-active");
      toggle.classList.toggle("bi-list", !active);
      toggle.classList.toggle("bi-x", active);
      return;
    }

    // Links de âncora na mesma página, incluindo o botão scroll-top (href="#")
    const link = e.target.closest("a[href]");
    if (!link || link.target || e.ctrlKey || e.metaKey || e.shiftKey) return;
    const url = new URL(link.href, location.href);
    if (url.origin !== location.origin || url.pathname !== location.pathname) return;
    if (!link.getAttribute("href").includes("#")) return;

    e.preventDefault();
    e.stopPropagation();
    body.classList.remove("mobile-nav-active");
    document.querySelector(".mobile-nav-toggle")?.classList.replace("bi-x", "bi-list");
    scrollToHash(url.hash);
    history.replaceState(null, "", url.hash || location.pathname);
  }, true);

  document.addEventListener("scroll", updatePageState, { passive: true });
  window.addEventListener("resize", updatePageState, { passive: true });
  window.addEventListener("load", updatePageState);

  // ---- Depois de cada navegação interna do Blazor ----
  window.Blazor?.addEventListener("enhancedload", () => {
    window.AOS?.refreshHard();
    updatePageState();
    if (location.hash) setTimeout(() => scrollToHash(location.hash), 100);
  });

  updatePageState();
  if (location.hash) setTimeout(() => scrollToHash(location.hash), 100);
})();