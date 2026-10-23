window.copiarTexto = async function (texto, event) {
    try {
        await navigator.clipboard.writeText(texto);

        let popup = document.getElementById("email-copy-popup");

        if (!popup) {
            popup = document.createElement("div");
            popup.id = "email-copy-popup";
            popup.className = "email-copy-popup";
            document.body.appendChild(popup);
        }

        popup.textContent = "E-mail copiado!";

        // Posiciona próximo ao mouse
        popup.style.left = `${event.clientX + 12}px`;
        popup.style.top = `${event.clientY + 12}px`;
        popup.style.bottom = "auto";
        popup.style.transform = "none";

        popup.classList.add("show");

        clearTimeout(popup.timeout);
        popup.timeout = setTimeout(() => {
            popup.classList.remove("show");
        }, 2000);

    } catch (error) {
        console.error("Erro ao copiar e-mail:", error);
    }
};
