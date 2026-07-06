/* ==========================================================================
   Lodestone — "Field Notes" editorial motion
   Lenis smooth scroll + GSAP / ScrollTrigger, with graceful fallbacks.
   Motion is quiet and typeset: line reveals, rule draws, plot stroke-draw.
   ========================================================================== */
(function () {
    "use strict";

    var doc = document;
    var body = doc.body;

    var prefersReduced = window.matchMedia &&
        window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    var hasGsap = typeof window.gsap !== "undefined";
    var hasST = hasGsap && typeof window.ScrollTrigger !== "undefined";
    var hasLenis = typeof window.Lenis !== "undefined";

    function showAll() { body.classList.add("no-motion"); }

    /* ----------------------------------------------------------------- Nav */
    function initNavbar() {
        var nav = doc.getElementById("nav");
        if (!nav) return;
        var toggle = function () {
            var y = window.scrollY || window.pageYOffset || 0;
            nav.classList.toggle("is-scrolled", y > 24);
        };
        toggle();
        window.addEventListener("scroll", toggle, { passive: true });
    }

    /* -------------------------------------------------------- Tab switching */
    function initTabs() {
        var tabs = Array.prototype.slice.call(doc.querySelectorAll(".tab[role='tab']"));
        if (!tabs.length) return;

        function select(tab) {
            tabs.forEach(function (t) {
                var selected = t === tab;
                t.setAttribute("aria-selected", selected ? "true" : "false");
                t.tabIndex = selected ? 0 : -1;
                var panel = doc.getElementById(t.getAttribute("aria-controls"));
                if (!panel) return;
                panel.classList.toggle("is-active", selected);
                if (selected) { panel.removeAttribute("hidden"); }
                else { panel.setAttribute("hidden", ""); }
            });
            if (window.ScrollTrigger) window.ScrollTrigger.refresh();
        }

        tabs.forEach(function (tab, i) {
            tab.addEventListener("click", function () { select(tab); });
            tab.addEventListener("keydown", function (e) {
                var dir = e.key === "ArrowRight" ? 1 : e.key === "ArrowLeft" ? -1 : 0;
                if (!dir) return;
                e.preventDefault();
                var next = tabs[(i + dir + tabs.length) % tabs.length];
                next.focus();
                select(next);
            });
        });
    }

    /* -------------------------------------------------------- Lenis scroll */
    function initLenis() {
        if (!hasLenis || prefersReduced) return null;
        var lenis = new window.Lenis({
            duration: 1.1,
            easing: function (t) { return Math.min(1, 1.001 - Math.pow(2, -10 * t)); },
            smoothWheel: true
        });

        if (hasST) {
            lenis.on("scroll", window.ScrollTrigger.update);
            window.gsap.ticker.add(function (time) { lenis.raf(time * 1000); });
            window.gsap.ticker.lagSmoothing(0);
        } else {
            var raf = function (time) { lenis.raf(time); requestAnimationFrame(raf); };
            requestAnimationFrame(raf);
        }
        return lenis;
    }

    /* Smooth-scroll for in-page anchors */
    function initAnchors(lenis) {
        doc.querySelectorAll('a[href^="#"]').forEach(function (link) {
            link.addEventListener("click", function (e) {
                var id = link.getAttribute("href");
                if (id.length < 2) return;
                var target = doc.querySelector(id);
                if (!target) return;
                e.preventDefault();
                if (lenis) { lenis.scrollTo(target, { offset: -74 }); }
                else { target.scrollIntoView({ behavior: prefersReduced ? "auto" : "smooth" }); }
            });
        });
    }

    /* --------------------------------------------------------- Animations */
    function initMotion() {
        var gsap = window.gsap;
        gsap.registerPlugin(window.ScrollTrigger);

        /* Hero headline — line by line, print rising into place */
        var heroLines = gsap.utils.toArray(".hero h1 .line > span");
        var tl = gsap.timeline({ defaults: { ease: "power3.out" } });
        if (heroLines.length) {
            gsap.set(heroLines, { yPercent: 115 });
            tl.to(heroLines, { yPercent: 0, duration: 0.9, stagger: 0.1 }, 0.1);
        }
        tl.from(".hero__kicker", { y: 14, opacity: 0, duration: 0.6 }, 0.1)
            .from(".hero__sub", { y: 18, opacity: 0, duration: 0.7 }, 0.5)
            .from(".hero__cta", { y: 18, opacity: 0, duration: 0.7 }, 0.62)
            .from(".hero__stat", { y: 14, opacity: 0, duration: 0.6, stagger: 0.09 }, 0.74)
            .from(".hero__visual", { y: 34, opacity: 0, duration: 1 }, 0.4);
        gsap.set(".hero .reveal", { clearProps: "opacity,transform" });

        /* Gentle float on the hero figure */
        var fig = doc.getElementById("hero-figure");
        if (fig) {
            gsap.to(fig, { y: -12, duration: 3.6, ease: "sine.inOut", yoyo: true, repeat: -1 });
        }

        /* Hero plot: draw line + fade area, then signal dots travel */
        drawStroke(".plot__line", gsap, null, 1.6, 0.7);
        var area = doc.querySelector(".plot__area");
        if (area) gsap.from(area, { opacity: 0, duration: 1, delay: 0.9 });
        buildSignals(gsap);

        /* Generic scroll reveals (skip hero — timeline owns it) */
        gsap.utils.toArray(".reveal").forEach(function (el) {
            if (el.closest(".hero")) return;
            gsap.fromTo(el,
                { y: 24, opacity: 0 },
                { y: 0, opacity: 1, duration: 0.75, ease: "power2.out",
                  scrollTrigger: { trigger: el, start: "top 88%", once: true } });
        });

        /* Staggered groups */
        [".problem-list", ".steps", ".features", ".stack", ".principles"].forEach(function (sel) {
            var group = doc.querySelector(sel);
            if (!group) return;
            gsap.fromTo(group.querySelectorAll(".reveal"),
                { y: 26, opacity: 0 },
                { y: 0, opacity: 1, duration: 0.7, ease: "power2.out", stagger: 0.08,
                  scrollTrigger: { trigger: group, start: "top 82%", once: true } });
        });

        /* Steps progress rail draws left-to-right */
        var rail = doc.querySelector(".steps__rail");
        if (rail) {
            gsap.fromTo(rail, { scaleX: 0 }, {
                scaleX: 1, duration: 1.1, ease: "power2.inOut",
                scrollTrigger: { trigger: ".steps", start: "top 74%", once: true }
            });
        }

        /* Admin trend — draw on scroll */
        drawStroke(".dplot__line", gsap, ".preview", 1.6, 0);

        /* CTA rule expands from center */
        var ctaRule = doc.querySelector(".cta__rule");
        if (ctaRule) {
            gsap.fromTo(ctaRule, { scaleX: 0 }, {
                scaleX: 1, duration: 0.8, ease: "power2.out",
                scrollTrigger: { trigger: ctaRule, start: "top 90%", once: true }
            });
        }
    }

    /* Draw an SVG path via stroke-dashoffset */
    function drawStroke(selector, gsap, triggerSel, dur, delay) {
        var el = doc.querySelector(selector);
        if (!el || typeof el.getTotalLength !== "function") return;
        var len = el.getTotalLength();
        gsap.set(el, { strokeDasharray: len, strokeDashoffset: len });
        gsap.to(el, {
            strokeDashoffset: 0,
            duration: dur || 1.6,
            ease: "power2.inOut",
            delay: triggerSel ? 0 : (delay || 0),
            scrollTrigger: triggerSel ? { trigger: triggerSel, start: "top 68%", once: true } : undefined
        });
    }

    /* Build + animate signal dots travelling across the hero figure
       (activity plot -> the "check-in suggested" note). Injected so the
       markup stays clean and dots never show without JS. */
    function buildSignals(gsap) {
        var host = doc.querySelector("#hero-figure .signals");
        if (!host) return;
        for (var i = 0; i < 4; i++) {
            var dot = doc.createElement("span");
            dot.className = "signal-dot";
            host.appendChild(dot);
        }
        /* inline style so no extra CSS rule is needed for the injected dots */
        var dots = host.querySelectorAll(".signal-dot");
        dots.forEach(function (dot, i) {
            dot.style.cssText =
                "position:absolute;width:7px;height:7px;border-radius:50%;" +
                "background:var(--clay);opacity:0;pointer-events:none;";
            var timeline = gsap.timeline({ repeat: -1, delay: i * 0.95 });
            timeline
                .set(dot, { left: "16%", top: "34%", scale: 0.6, opacity: 0 })
                .to(dot, { opacity: 1, scale: 1, duration: 0.35 })
                .to(dot, { left: "22%", top: "78%", duration: 2.3, ease: "power1.inOut" }, "<")
                .to(dot, { opacity: 0, scale: 0.5, duration: 0.35 }, "-=0.3")
                .to({}, { duration: 1.1 });
        });
        host.style.cssText = "position:absolute;inset:0;overflow:hidden;pointer-events:none;";
    }

    /* ------------------------------------------------------------- Bootstrap */
    function start() {
        initNavbar();
        initTabs();

        if (prefersReduced || !hasGsap || !hasST) {
            showAll();
            initAnchors(null);
            return;
        }

        body.classList.add("js-ready");
        var lenis = initLenis();
        initAnchors(lenis);
        initMotion();
        window.ScrollTrigger.refresh();
    }

    if (doc.readyState === "loading") {
        doc.addEventListener("DOMContentLoaded", start);
    } else {
        start();
    }
})();
