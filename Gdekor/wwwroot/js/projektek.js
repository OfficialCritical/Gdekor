document.addEventListener('DOMContentLoaded', function () {

    const animatedDiv = document.querySelector('.animatedDiv');
    const form_Projekt = document.getElementById('form_Projekt');

    const bttn_uj = document.querySelector('.bttn_uj');
    const p_Valaszto = document.getElementById('p_Valaszto');
    const p_reszletek = document.getElementById('p_reszletek');

    const szTipus_select = document.getElementById('szTipus_select');
    const emHozza_select = document.getElementById('emHozza_select');
    const userLista_Tbl_Bdy = document.querySelector('.userLista_Tbl tbody');

    if (!p_Valaszto || !p_reszletek) return;

    if (animatedDiv) {
        animatedDiv.classList.add('elohiv');
    }

    const vanHiba = document.querySelector('.validation-summary-errors')
        || document.querySelector('span.field-validation-error');

    if (vanHiba) {
        document.querySelectorAll('.cim_Div').forEach(function (cim) {
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            reszletek.classList.add('elohiv');
            nyil.classList.add('srehen');
        });
        p_reszletek.classList.add('aktiv');

        const proId = document.getElementById('Pro_ID_Edit')?.value;
        if (proId) {
            p_Valaszto.value = proId;
            betoltResztvevok(proId);
        }
    }

    function mezoBeallit(id, ertek) {
        const inp = document.getElementById(id);
        if (!inp) return;

        const value = ertek?.trim() ?? '';
        const fp = window.datumFp?.[id];

        if (fp && typeof fp.setDate === 'function') {
            if (value) {
                fp.setDate(value, false, 'Y-m-d');
            } else {
                fp.clear();
            }
        } else {
            inp.value = value;
        }
    }

    function resztvevoSorLetrehoz(r) {
        const tr = document.createElement('tr');
        tr.dataset.id = r.id || '';
        tr.dataset.userId = r.userId || '';

        const egyeni = szTipus_select?.value === 'egyeni';

        tr.innerHTML = `
            <td>
                ${egyeni ? '<button type="button" class="btn btn-sm btn-outline-danger bttn_userTorol">törlés</button>' : ''}
            </td>
            <td class="userNev">${r.nev ?? ''}</td>
            <td>
                <input type="number" class="form-control userOraber" min="0" placeholder="Ft" value="${r.oraber ?? ''}">
            </td>
            <td>
                <input type="number" class="form-control userNapiber" min="0" placeholder="Ft" value="${r.napiber ?? ''}">
            </td>
        `;

        return tr;
    }

    function getResztvevokAktualis() {
        if (!userLista_Tbl_Bdy) return [];

        return [...userLista_Tbl_Bdy.querySelectorAll('tr')].map(tr => ({
            id: tr.dataset.id || '',
            userId: tr.dataset.userId || '',
            nev: tr.querySelector('.userNev')?.textContent?.trim() ?? '',
            oraber: tr.querySelector('.userOraber')?.value ?? '',
            napiber: tr.querySelector('.userNapiber')?.value ?? ''
        }));
    }

    function renderResztvevoTabla(lista) {
        if (!userLista_Tbl_Bdy) return;

        userLista_Tbl_Bdy.innerHTML = '';

        if (!lista.length) return;

        lista.forEach(r => {
            userLista_Tbl_Bdy.appendChild(resztvevoSorLetrehoz(r));
        });
    }

    function resztvevoListaUrit() {
        if (userLista_Tbl_Bdy) {
            userLista_Tbl_Bdy.innerHTML = '';
        }
    }

    async function betoltResztvevok(proId) {
        if (!proId) {
            resztvevoListaUrit();
            return;
        }

        const url = `/G_Oldalak/Projektek?handler=Resztvevok&proId=${encodeURIComponent(proId)}`;
        const resp = await fetch(url, { headers: { 'Accept': 'application/json' } });

        if (!resp.ok) {
            resztvevoListaUrit();
            return;
        }

        const lista = await resp.json();
        renderResztvevoTabla(lista);
    }

    function resztvevokJsonBeallit() {
        const hidden = document.getElementById('Resztvevok_Json');
        if (!hidden) return;

        hidden.value = JSON.stringify(getResztvevokAktualis());
    }

    function userSorHozzaad(option) {
        if (!userLista_Tbl_Bdy || !option?.value) return;

        const letezik = userLista_Tbl_Bdy.querySelector(`tr[data-user-id="${option.value}"]`);
        if (letezik) return;

        const tr = document.createElement('tr');
        tr.dataset.userId = option.value;
        tr.dataset.id = '';

        if (szTipus_select.value === 'mindenki') {
            tr.innerHTML = `
                <td></td>
                <td class="userNev">${option.textContent}</td>
                <td>
                    <input type="number" class="form-control userOraber" min="0" placeholder="Ft">
                </td>
                <td>
                    <input type="number" class="form-control userNapiber" min="0" placeholder="Ft">
                </td>
            `;
        } else {
            tr.innerHTML = `
                <td>
                    <button type="button" class="btn btn-sm btn-outline-danger bttn_userTorol">törlés</button>
                </td>
                <td class="userNev">${option.textContent}</td>
                <td>
                    <input type="number" class="form-control userOraber" min="0" placeholder="Ft">
                </td>
                <td>
                    <input type="number" class="form-control userNapiber" min="0" placeholder="Ft">
                </td>
            `;
        }

        userLista_Tbl_Bdy.appendChild(tr);
    }

    function szTipusFrissit() {
        if (!szTipus_select || !userLista_Tbl_Bdy) return;

        userLista_Tbl_Bdy.innerHTML = '';

        if (szTipus_select.value === 'mindenki') {
            if (emHozza_select) {
                Array.from(emHozza_select.options).forEach(option => {
                    if (option.value === '') return;
                    userSorHozzaad(option);
                });
                emHozza_select.classList.remove('elohiv');
            }
        } else if (szTipus_select.value === 'egyeni') {
            emHozza_select?.classList.add('elohiv');
        } else {
            emHozza_select?.classList.remove('elohiv');
        }
    }

    p_Valaszto.addEventListener('change', function () {
        const opt = p_Valaszto.selectedOptions[0];

        if (p_Valaszto.value) {
            bttn_uj?.classList.remove('aktiv');

            p_reszletek.classList.add('aktiv');
            document.querySelectorAll('.cim_Div').forEach(function (cim) {
                const reszletek = cim.nextElementSibling;
                const nyil = cim.querySelector('.forgoNyil');

                reszletek.classList.add('elohiv');
                nyil.classList.add('srehen');
            });

            const d = opt.dataset;

            document.getElementById('Pro_ID_Edit').value = p_Valaszto.value;
            document.getElementById('Nev_Edit').value = d.nev ?? '';
            document.getElementById('Allapot_Edit').value = d.allapot ?? '';
            document.getElementById('Leir_Edit').value = d.leir ?? '';
            document.getElementById('szTipus_select').value = d.kik ?? '';

            document.getElementById('Bevetel_Edit').value = d.bevetel ?? '';
            document.getElementById('Koltseg_Edit').value = d.koltseg ?? '';
            document.getElementById('Profit_Edit').value = d.profit ?? '';

            mezoBeallit('TervKezd_Edit', d.tervKezd);
            mezoBeallit('TervVeg_Edit', d.tervVeg);
            mezoBeallit('ValosKezd_Edit', d.valosKezd);
            mezoBeallit('ValosVeg_Edit', d.valosVeg);

            if (d.kik === 'egyeni') {
                emHozza_select?.classList.add('elohiv');
            } else {
                emHozza_select?.classList.remove('elohiv');
            }

            betoltResztvevok(p_Valaszto.value);
        } else {
            form_Projekt.reset();
            document.getElementById('Pro_ID_Edit').value = '';
            p_reszletek.classList.remove('aktiv');
            resztvevoListaUrit();
        }
    });

    bttn_uj?.addEventListener('click', function () {
        form_Projekt.reset();
        document.getElementById('Pro_ID_Edit').value = '';
        bttn_uj.classList.add('aktiv');
        p_reszletek.classList.add('aktiv');

        document.querySelectorAll('.cim_Div').forEach(function (cim) {
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            reszletek.classList.add('elohiv');
            nyil.classList.add('srehen');
        });

        resztvevoListaUrit();
        emHozza_select?.classList.remove('elohiv');
    });

    if (szTipus_select) {
        szTipus_select.addEventListener('change', szTipusFrissit);
    }

    if (emHozza_select) {
        emHozza_select.addEventListener('change', function () {
            if (szTipus_select?.value !== 'egyeni') return;

            const option = emHozza_select.selectedOptions[0];
            if (!option?.value) return;

            userSorHozzaad(option);
            emHozza_select.value = '';
        });
    }

    if (userLista_Tbl_Bdy) {
        userLista_Tbl_Bdy.addEventListener('click', function (e) {
            const torolGomb = e.target.closest('.bttn_userTorol');
            if (!torolGomb) return;
            torolGomb.closest('tr')?.remove();
        });
    }

    if (form_Projekt) {
        form_Projekt.addEventListener('submit', function () {
            resztvevokJsonBeallit();
        });
    }

    document.querySelectorAll('.cim_Div').forEach(function (cim) {
        cim.addEventListener('click', function () {
            const reszletek = cim.nextElementSibling;
            const nyil = cim.querySelector('.forgoNyil');

            if (!reszletek || !reszletek.classList.contains('reszletek_Div')) return;

            if (reszletek.classList.contains('elohiv')) {
                reszletek.classList.remove('elohiv');
                nyil.classList.remove('srehen');
            } else {
                reszletek.classList.add('elohiv');
                nyil.classList.add('srehen');
            }
        });
    });

});