
const key = document.querySelector("svg.key");
const keyhole = document.querySelector("svg.keyhole");
const ghost = document.querySelector(".ghost");
const heading = document.querySelector(".error-container h1");
const paragraph = document.querySelector(".error-container p");

const rootStyles = getComputedStyle(document.documentElement);
const animDurSec = parseFloat(rootStyles.getPropertyValue("--anim-duration")) * 1000;
const triggerTime = animDurSec * 9 / 8;

setTimeout(() => {
    document.body.style.cursor = "grab";

    key.style.animationPlayState = "running";
    keyhole.style.animationPlayState = "running";
    key.style.pointerEvents = "none";

    const box = key.getBoundingClientRect();
    function updateKey(e) {
        key.style.left = `${e.clientX - box.width / 2}px`;
        key.style.top = `${e.clientY - box.height / 2}px`;
    }
    window.addEventListener("mousemove", updateKey);

    keyhole.addEventListener("mouseover", () => {
        document.body.style.cursor = "default";
        heading.textContent = "🎉 yay 🎉";
        paragraph.textContent = "access granted";
        key.style.display = "none";
        keyhole.style.display = "none";
        window.removeEventListener("mousemove", updateKey);
    });

}, triggerTime);
