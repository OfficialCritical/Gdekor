document.addEventListener('DOMContentLoaded', function () {

    const naptarEl = document.getElementById('naptar');
    const valasszDatumot = document.querySelector('.valasszDatumot');
    const form_mOrak = document.getElementById('form_mOrak');
    const form_Belso = document.querySelector('.form_Belso');
    const select_Projekt = document.getElementById('select_Projekt');

    if (!naptarEl || !form_mOrak || !valasszDatumot || !form_Belso || !select_Projekt) return;

    let kivalasztottDatum = null;

    function maStrLocal() {
        const d = new Date();
        const ev = d.getFullYear();
        const ho = String(d.getMonth() + 1).padStart(2, '0');
        const nap = String(d.getDate()).padStart(2, '0');
        return `${ev}-${ho}-${nap}`;
    }

    function torolKijeloles() {
        document.querySelectorAll('.fc-daygrid-day.kivalasztott-nap')
            .forEach(el => el.classList.remove('kivalasztott-nap'));
    }

    function frissitMaiHatter() {
        const maCella = naptarEl.querySelector('.fc-day-today');
        if (!maCella) return;

        if (kivalasztottDatum && kivalasztottDatum !== maStrLocal()) {
            maCella.classList.add('mai-visszaallitva');
        } else {
            maCella.classList.remove('mai-visszaallitva');
        }
    }

    function kijelolNap(info) {
        torolKijeloles();

        info.dayEl.classList.add('kivalasztott-nap');
        kivalasztottDatum = info.dateStr;

        frissitMaiHatter();
    }

    const calendar = new FullCalendar.Calendar(naptarEl, {
        initialView: 'dayGridMonth',
        locale: 'hu',
        firstDay: 1,
        height: 'auto',
        fixedWeekCount: false,

        dateClick: function (info) {
            if (!form_mOrak.classList.contains('elohiv')) {
                valasszDatumot.classList.add('elrejt');
                form_mOrak.classList.add('elohiv');
            }

            kijelolNap(info);
        },

        datesSet: function () {
            if (!kivalasztottDatum) {
                frissitMaiHatter();
                return;
            }

            const cella = naptarEl.querySelector(`[data-date="${kivalasztottDatum}"]`);
            if (!cella) {
                torolKijeloles();
                kivalasztottDatum = null;
                frissitMaiHatter();
                return;
            }

            torolKijeloles();
            cella.classList.add('kivalasztott-nap');
            frissitMaiHatter();
        },

        customButtons: {
            honapFel: {
                text: '↿',
                click: function () {
                    calendar.prev();
                }
            },
            honapLe: {
                text: '⇂',
                click: function () {
                    calendar.next();
                }
            }
        },

        headerToolbar: {
            left: 'honapFel honapLe',
            center: '',
            right: 'title'
        },

        titleFormat: { year: 'numeric', month: 'long' }
    });

    calendar.render();

    select_Projekt.addEventListener('change', function () {
        if (select_Projekt.value) {
            form_Belso.classList.add('aktiv');
        } else {
            form_Belso.classList.remove('aktiv');
        }
    });
});