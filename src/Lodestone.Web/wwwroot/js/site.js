/* Lodestone — shared shell script */
(function () {
    'use strict';

    var doc = document;
    var body = doc.body;

    var prefersReduced = window.matchMedia &&
        window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    /* ── Smooth scroll (Lenis) — same feel as the landing page ────────── */
    function initLenis() {
        if (!window.Lenis || prefersReduced) return null;
        try {
            var lenis = new window.Lenis({
                duration: 1.1,
                easing: function (t) { return Math.min(1, 1.001 - Math.pow(2, -10 * t)); },
                smoothWheel: true
            });

            var raf = function (time) {
                lenis.raf(time);
                requestAnimationFrame(raf);
            };
            requestAnimationFrame(raf);
            return lenis;
        } catch (err) {
            if (window.console && window.console.warn) {
                window.console.warn('Lenis failed to initialize; using native scrolling.', err);
            }
            return null;
        }
    }

    /* Smooth-scroll for in-page anchors (e.g. "#privacy") */
    function initAnchors(lenis) {
        doc.querySelectorAll('a[href^="#"]').forEach(function (link) {
            var id = link.getAttribute('href');
            if (!id || id.length < 2) return;
            link.addEventListener('click', function (e) {
                var target = doc.querySelector(id);
                if (!target) return;
                e.preventDefault();
                if (lenis) {
                    lenis.scrollTo(target, { offset: -72 });
                } else {
                    target.scrollIntoView({ behavior: prefersReduced ? 'auto' : 'smooth' });
                }
            });
        });
    }

    var lenisInstance = initLenis();
    initAnchors(lenisInstance);

    /* ── Navbar: scroll class ─────────────────────────────────────────── */
    var nav = doc.getElementById('ls-nav');
    if (nav) {
        var onScroll = function () { nav.classList.toggle('is-scrolled', window.scrollY > 12); };
        window.addEventListener('scroll', onScroll, { passive: true });
        if (lenisInstance) lenisInstance.on('scroll', onScroll);
        onScroll();
    }

    /* ── Navbar: hamburger toggle ─────────────────────────────────────── */
    var toggle = nav && nav.querySelector('.ls-nav__toggle');
    var mobileMenu = nav && nav.querySelector('.ls-nav__menu');
    if (toggle && mobileMenu) {
        toggle.addEventListener('click', function () {
            var expanded = toggle.getAttribute('aria-expanded') === 'true';
            toggle.setAttribute('aria-expanded', String(!expanded));
            nav.classList.toggle('is-open', !expanded);
        });

        // Close on outside click
        doc.addEventListener('click', function (e) {
            if (nav.classList.contains('is-open') && !nav.contains(e.target)) {
                nav.classList.remove('is-open');
                toggle.setAttribute('aria-expanded', 'false');
            }
        });

        // Close on Escape
        doc.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && nav.classList.contains('is-open')) {
                nav.classList.remove('is-open');
                toggle.setAttribute('aria-expanded', 'false');
                toggle.focus();
            }
        });
    }

    /* ── Navbar: active link highlight ───────────────────────────────── */
    var path = window.location.pathname.toLowerCase();
    var navLinks = nav && nav.querySelectorAll('.ls-nav__links a');
    if (navLinks) {
        navLinks.forEach(function (a) {
            var href = a.getAttribute('href') || '';
            if (href !== '/' && path.indexOf(href.toLowerCase()) === 0) {
                a.classList.add('is-active');
            }
        });
    }

})();
