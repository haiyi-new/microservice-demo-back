#!/bin/sh
# wait-for-rabbitmq.sh

set -e

# Use the value of the RABBITMQ_HOST environment variable, or "localhost" if it's not set
host="${RABBITMQ_HOST:-localhost}"
cmd="$@"

until nc -z $host 5672; do
  >&2 echo "RabbitMQ is unavailable - sleeping"
  sleep 1
done

>&2 echo "RabbitMQ is up - executing command"
>&2 echo "command is $cmd"

exec $cmd