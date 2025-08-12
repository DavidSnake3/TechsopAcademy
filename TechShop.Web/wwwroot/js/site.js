// site.js

window.showSpinner = function () {
    document.getElementById('spinner-overlay')
        .classList.remove('hidden');
};
window.hideSpinner = function () {
    document.getElementById('spinner-overlay')
        .classList.add('hidden');
};

// Cuando la página empieza a descargarse (antes de navegar away)
window.addEventListener('beforeunload', showSpinner);
// Cuando ya cargó completamente
window.addEventListener('load', hideSpinner);
