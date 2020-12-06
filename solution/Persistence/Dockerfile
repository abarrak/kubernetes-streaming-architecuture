FROM alpine:3.7
LABEL stage=builder

RUN apk add --update sqlite
ADD migrate.sh /

WORKDIR /
ENTRYPOINT ["/bin/sh", "migrate.sh"]
