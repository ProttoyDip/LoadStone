(function () {
    'use strict';

    var note = document.getElementById('NewEntry_Note');
    var count = document.getElementById('journal-note-count');

    if (note && count) {
        var updateCount = function () { count.textContent = String(note.value.length); };
        note.addEventListener('input', updateCount);
        updateCount();
    }

    document.querySelectorAll('.journal-local-datetime').forEach(function (element) {
        var value = element.getAttribute('datetime');
        if (!value) return;
        var date = new Date(value);
        if (Number.isNaN(date.getTime())) return;
        element.textContent = new Intl.DateTimeFormat(undefined, {
            dateStyle: 'medium',
            timeStyle: 'short'
        }).format(date);
    });

    document.querySelectorAll('.journal-local-date').forEach(function (element) {
        var value = element.getAttribute('datetime');
        if (!value) return;
        var date = new Date(value);
        if (Number.isNaN(date.getTime())) return;
        element.textContent = new Intl.DateTimeFormat(undefined, {
            day: '2-digit',
            month: 'short'
        }).format(date);
    });
})();
