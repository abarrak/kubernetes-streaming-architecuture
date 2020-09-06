FROM alpine:3.7
RUN apk add --update sqlite
RUN mkdir /file-store
RUN mkdir /db
RUN mkdir /db/migrations
WORKDIR /db

ENTRYPOINT ["sqlite3"]
CMD ["database.db"]
