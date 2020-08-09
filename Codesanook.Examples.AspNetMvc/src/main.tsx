import './sass/style.scss';
import {PushMessageFrontend, PushMessageBackend, ConnectionProvider} from  './components';
// https://stackoverflow.com/a/63151271/1872200
import React from 'react';
import { render } from 'react-dom';

const App = () => {
    return (
        <ConnectionProvider>
            <PushMessageFrontend/>
            <hr/>
            <PushMessageBackend/>
        </ConnectionProvider>
    );
};

render(
    <App/>,
    document.getElementById('pushMessage')
);

/*
import 'jquery-validation';
import 'jquery-validation-unobtrusive';
TODO move to a new file
declare namespace JQueryValidation {
    interface Validator {
        invalid: any;
    }
}

const form = $('form');
$.validator.setDefaults({

    onsubmit: false,
    //onfocusout: element => {
    //    var validator = form.data('validator') as JQueryValidation.Validator;
    //    validator.element($(element));
    //    console.log('focus out');
    //}
})

form.submit(e => {
    e.preventDefault();
    console.log('submitted');
    var validator = form.data('validator') as JQueryValidation.Validator;
    //validator.settings.invalidHandler = (error, va) => {
    //    console.log
    //    console.log(error);
    //};

    //https://stackoverflow.com/a/50066345/1872200
    var result = validator.form();
    console.log(`validated result ${result}`);
    validator.showErrors({ FirstName: 'error' });
    validator.invalid['FirstName'] = false;
    //if (result) {
    //    form.get(0).submit();
    //}
});
 */

