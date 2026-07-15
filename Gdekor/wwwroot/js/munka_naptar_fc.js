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
        select_Projekt.value = '';
        document.getElementById('m_kezdete').value = '';
        document.getElementById('m_vege').value = '';
        form_Belso.classList.remove('aktiv');
        info.dayEl.classList.add('kivalasztott-nap');
        kivalasztottDatum = info.dateStr;
        document.getElementById('Datum_Edit').value = info.dateStr;
        frissitMaiHatter();
    }
    function napValtas(datumStr) {
        document.getElementById('Datum_Edit').value = datumStr;
        document.getElementById('MNap_ID_Edit').value = '';
        document.getElementById('m_kezdete').value = '';
        document.getElementById('m_vege').value = '';

        const projektId = document.getElementById('select_Projekt').value;
        if (projektId) {
            betoltMunkaOra(datumStr, projektId);
        }
    }
    const calendar = new FullCalendar.Calendar(naptarEl, {
        initialView: 'dayGridMonth',
        locale: 'hu',
        firstDay: 1,
        height: 'auto',
        fixedWeekCount: false,

        dateClick: function (info) {
            if (info.dayEl.classList.contains('fc-day-other')) {
                return;
            }
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

    async function betoltMunkaOra(datum, projektId) {
        const url = `/FelhOldalak/Munkaoraim?handler=Betoltes&datum=${encodeURIComponent(datum)}&projektId=${encodeURIComponent(projektId)}`;

        const resp = await fetch(url, {
            headers: { 'Accept': 'application/json' }
        });

        if (!resp.ok) return;

        const data = await resp.json();

        document.getElementById('Datum_Edit').value = data.datum;

        if (data.vanMentett) {
            document.getElementById('MNap_ID_Edit').value = data.mNapId;
            document.getElementById('m_kezdete').value = data.kezdete ?? '';
            document.getElementById('m_vege').value = data.vege ?? '';
        } else {
            document.getElementById('MNap_ID_Edit').value = data.mNapId ?? ''; // ha munkanap van, de óra még nincs
            document.getElementById('m_kezdete').value = '';
            document.getElementById('m_vege').value = '';
        }
    }

    select_Projekt.addEventListener('change', function () {
        const datum = document.getElementById('Datum_Edit').value;

        if (select_Projekt.value && datum) {
            betoltMunkaOra(datum, select_Projekt.value);
        } else {
            document.getElementById('MNap_ID_Edit').value = '';
            document.getElementById('m_kezdete').value = '';
            document.getElementById('m_vege').value = '';
        }

        // meglévő form_Belso aktiválás maradhat
        form_Belso.classList.toggle('aktiv', !!select_Projekt.value);
    });
});