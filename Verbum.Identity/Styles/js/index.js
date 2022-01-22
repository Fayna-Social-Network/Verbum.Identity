const inputs = document.querySelectorAll('.form-control');
const infoSpans = document.querySelectorAll('.info-span');
const messageBox = document.getElementById('message-box');
const submit = document.getElementById('submitButton');




inputs.forEach((item) => {
    item.addEventListener('click', clearErrors);
})


function clearErrors(){
    infoSpans.forEach((item) => {
        item.innerHTML = '';
    })

    if (messageBox.classList.contains('message-box-active')) {
        messageBox.classList.remove('message-box-active')
    }
   
   
}


document.addEventListener('DOMContentLoaded', () => {
    let messElement = document.querySelectorAll('.validation-summary-errors');

    if (messElement.length != 0) {
        messageBox.classList.add('message-box-active')
    }
})

