FROM alpine:3.7
LABEL stage=builder

RUN apk add --update sqlite

RUN mkdir /usr/db/
ADD migrate.sh /usr/db/
RUN chmod -R 777 /usr/db/

ENTRYPOINT ["/bin/sh", "/user/db/migrate.sh"]
