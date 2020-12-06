FROM alpine:3.7
LABEL stage=builder


RUN apk add --update sqlite
ADD migrate.sh /
ADD db/migrations/01_initial_script.sql /
ADD db/migrations/02_add_locking_column_to_mediafile_table.sql /

RUN chmod -R 777 /migrate.sh

WORKDIR /
ENTRYPOINT ["/bin/sh", "migrate.sh"]
