import './sass/style.scss'
import 'jquery';
import 'jquery-validation';
import 'jquery-validation-unobtrusive';


const form = $('form');

$.validator.setDefaults({
    
    onsubmit: false,
    onfocusout: element => {
        var validator = form.data('validator') as JQueryValidation.Validator;
        validator.element($(element));
    }
})

form.submit(e => {
    e.preventDefault();
    console.log('submitted');
    var validator = form.data('validator') as JQueryValidation.Validator;
    validator.settings.invalidHandler = (error, va) => {
        console.log(error);
    };

    //https://stackoverflow.com/a/50066345/1872200
    var result = validator.form();
});
