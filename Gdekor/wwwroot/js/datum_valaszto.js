
window.datumFp = {};  

document.querySelectorAll('.datumvalaszto').forEach(dv => {
    window.datumFp[dv.id] = flatpickr(dv, {
        locale: 'hu',
        dateFormat: 'Y-m-d',
        allowInput: false
    });
});