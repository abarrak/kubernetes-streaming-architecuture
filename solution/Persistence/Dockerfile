FROM alpine:3.7
LABEL stage=builder


RUN apk add --update sqlite
ADD migrate.sh /

RUN chmod -R 777 /migrate.sh

WORKDIR /
ENTRYPOINT ["/bin/sh", "migrate.sh"]
