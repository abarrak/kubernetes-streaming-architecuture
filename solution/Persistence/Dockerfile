FROM alpine:3.7
LABEL stage=builder

RUN apk add --update sqlite
RUN mkdir /usr/db/
ADD migrate.sh /user/db/

WORKDIR /user/db/
ENTRYPOINT ["/bin/sh", "migrate.sh"]
