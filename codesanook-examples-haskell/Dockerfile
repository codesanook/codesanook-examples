FROM ubuntu:16.04

VOLUME /code
WORKDIR /code

RUN apt-get update && apt-get install -qy haskell-platform
RUN cabal update && cabal install hspec

ENTRYPOINT ["runhaskell"]
