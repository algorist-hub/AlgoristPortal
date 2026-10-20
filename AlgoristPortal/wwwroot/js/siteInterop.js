export function setMobileNavigation(isActive) {
    document.body.classList.toggle("mobile-nav-active", isActive);
}

export function initializePageAnimations() {
    window.AOS?.init({
        duration: 600,
        easing: "ease-in-out",
        once: true,
        mirror: false
    });
}

let pageInteractionController;

export function initializePageInteractions() {
    pageInteractionController?.abort();
    pageInteractionController = new AbortController();

    const { signal } = pageInteractionController;
    const body = document.body;
    const header = document.querySelector("#header");
    const scrollTop = document.querySelector("#scroll-top");
    const navLinks = document.querySelectorAll(".navmenu a");

    function updateScrollState() {
        if (header && (header.classList.contains("scroll-up-sticky") || header.classList.contains("sticky-top") || header.classList.contains("fixed-top"))) {
            body.classList.toggle("scrolled", window.scrollY > 100);
        }

        scrollTop?.classList.toggle("active", window.scrollY > 100);
    }

    function updateActiveNavigation() {
        const position = window.scrollY + 200;
        let activeLink = null;

        navLinks.forEach((link) => {
            if (!link.hash) {
                return;
            }

            const section = document.querySelector(link.hash);
            if (section && position >= section.offsetTop && position <= section.offsetTop + section.offsetHeight) {
                activeLink = link;
            }
        });

        if (window.scrollY + window.innerHeight >= document.documentElement.scrollHeight - 1 && navLinks.length > 0) {
            activeLink = navLinks[navLinks.length - 1];
        }

        navLinks.forEach((link) => link.classList.toggle("active", link === activeLink));
    }

    function updatePageState() {
        updateScrollState();
        updateActiveNavigation();
    }

    function scrollToCurrentFragment() {
        const section = document.getElementById(decodeURIComponent(window.location.hash.slice(1)));
        if (!section) {
            return;
        }

        const scrollMarginTop = Number.parseInt(getComputedStyle(section).scrollMarginTop, 10) || 0;
        window.scrollTo({
            top: section.offsetTop - scrollMarginTop,
            behavior: "smooth"
        });
    }

    document.addEventListener("scroll", updatePageState, { passive: true, signal });
    window.addEventListener("resize", updatePageState, { passive: true, signal });

    scrollTop?.addEventListener("click", (event) => {
        event.preventDefault();
        window.scrollTo({ top: 0, behavior: "smooth" });
    }, { signal });

    updatePageState();

    if (window.location.hash) {
        window.setTimeout(scrollToCurrentFragment, 100);
    }
}

export function disposePageInteractions() {
    pageInteractionController?.abort();
    pageInteractionController = undefined;
}