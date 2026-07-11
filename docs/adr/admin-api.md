# ADR-004 Public API vs Admin API

## Status

Accepted

## Context

Платформа предоставляет два типа доступа.

Владелец бизнеса управляет системой.

Клиенты записываются через Telegram.

## Decision

Разделяем API.

Admin API

- JWT
- React
- Business Context

Public API

- Telegram
- Без JWT владельца
- Контекст бизнеса определяется автоматически.

## Consequences

Плюсы

- Простая модель безопасности.
- Telegram не зависит от авторизации владельца.
- Легко добавить Web Booking.

Минусы

- Два разных сценария авторизации.