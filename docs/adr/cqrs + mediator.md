# ADR-002 CQRS with MediatR

## Status

Accepted

## Context

Необходимо разделить команды и запросы.

## Decision

Каждая бизнес-функция реализуется как Vertical Slice.

Каждый Slice содержит:

- Command/Query
- Validator
- Handler
- DTO/Response

Запуск осуществляется через MediatR.

## Consequences

Плюсы

- Независимые фичи.
- Простое тестирование.
- Минимальная связанность.

Минусы

- Больше файлов.