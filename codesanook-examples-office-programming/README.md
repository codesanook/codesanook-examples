# Google AppScript with clasp (Command Line Apps Script Projects)

- To create new project, we need to turn on Google AppScript API
- Go to https://script.google.com/home/usersettings and turn on the API

# Intall packages
- CD to the root folder of the current project then run
``` 
  yarn install 
```

- Log in to Google App
```
  yarn run clasp login
```

- For creating a new project
```
  yarn run clasp create -- --type standalone --title "mail-merge" --rootDir "src" --parentId "xxx"
```

- Push new changes
```
  yarn run clasp push
```

- Push with watch
```
  yarn run clasp push -- --watch
```

## Useful Google Sheet API tips
- [มาเขียน Google Apps Script บน local กันเถอะ (ใช้ modern javascript ได้ด้วยนะ](https://peerasak.com/post/modern-javascript-on-google-apps-script/)

- [Command Line Apps Script Projects (CLASP) - read me](https://github.com/google/clasp/blob/master/README.md)

https://github.com/google/clasp#setting

## Credit
- Peerasak Unsakon
