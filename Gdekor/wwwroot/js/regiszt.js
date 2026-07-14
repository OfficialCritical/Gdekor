document.addEventListener('DOMContentLoaded', function () {

    const animatedDiv = document.querySelector('.animatedDiv');

    const kozepResz = document.querySelector('.kozepResz');
    const bttnReg = document.getElementById('bttn_reg');
    const bttnLeDiv = document.getElementById('bttnLe_Div');
    const bttnLe = document.getElementById('bttnLe');

    const bttn_szList_Megtek = document.getElementById('bttn_szList_Megtek');

    if (!kozepResz || !bttnReg || !bttnLeDiv) return;

    animatedDiv.classList.add('elohiv');

    const observer = new IntersectionObserver(
        (entries) => {
            const reglatszik = entries[0].isIntersecting;
            const kellGorgetes = kozepResz.scrollHeight > kozepResz.clientHeight;

            bttnLeDiv.classList.toggle('aktiv', kellGorgetes && !reglatszik);
        },
        {
            root: kozepResz,
            threshold: 0.1
        }
    );

    observer.observe(bttnReg);

    bttnLe.addEventListener('click', function () {
        kozepResz.scrollTo({
            top: kozepResz.scrollHeight,
            behavior: 'smooth'
        });
    });

    bttn_szList_Megtek.addEventListener('click', function () {

    });
});