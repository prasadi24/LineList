const body = document.querySelector("body");
const darkLight = document.querySelector("#darkLight");
const sidebar = document.querySelector(".sidebar");
const submenuItems = document.querySelectorAll(".submenu_item");
const sidebarOpen = document.querySelector("#sidebarOpen");
sidebarOpen.addEventListener("click", () => sidebar.classList.toggle("close"));

sidebar.addEventListener("mouseenter", () => {
    if (sidebar.classList.contains("hoverable")) {
        sidebar.classList.remove("close");
    }
});
sidebar.addEventListener("mouseleave", () => {
    if (sidebar.classList.contains("hoverable")) {
        sidebar.classList.add("close");
    }
});

submenuItems.forEach((item, index) => {
    item.addEventListener("click", () => {
        item.classList.toggle("show_submenu");
        submenuItems.forEach((item2, index2) => {
            if (index !== index2) {
                item2.classList.remove("show_submenu");
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    let currentUrl = window.location.pathname;
    let navLinks = document.querySelectorAll(".nav_link");

    let activeLink = null;

    navLinks.forEach(link => {
        if (link.getAttribute("href") === currentUrl) {
            link.classList.add("active");
            activeLink = link; // Store reference to active link
        }
    });

    // If active link is inside the submenu, scroll it into view
    if (activeLink && activeLink.classList.contains("sublink")) {
        activeLink.scrollIntoView({ behavior: "smooth", block: "center" });
    }
});