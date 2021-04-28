# codesanook-examples-jekyll

## To run the project locally 

- Clone git@github.com:codesanook/Codesanook.Examples.git
- CD to `codesanook-examples-jekyll`        
- Run `docker-compose up` 
- Wait for a while, Docker is pulling Jekyll image and setting up a Jekyll site .
- If you find `Server address: http://0.0.0.0:4000/` message, it means Jekyll site is ready to use. 
- Open a browser and navigate to http://localhost:4000/.
- Change some contents in index.md or files in `_posts_` folder ane you will find hot reload work because Jekyll site has live reload endpoint at `ws://localhost:35729/livereload`.

## Test with other endpoints
- Testing with http://127.0.0.1:4000 work both website and hot reload.
- Testing with http://0.0.0.0:4000/ does not work at all. 
- Environment, code checked out and run in WSL2 on Windows 10 pro version 20H2, build 19042.928
