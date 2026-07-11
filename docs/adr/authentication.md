# ADR-003 Authentication

## Status

Accepted

## Context

Система является SaaS.

Один пользователь может владеть несколькими компаниями.

Необходимо обеспечить безопасную аутентификацию.

## Decision

Используем:

- JWT Access Token
- Refresh Token
- Rotation Refresh Token

Пароли хранятся только в виде Hash.

Refresh Token хранится в виде Hash.

JWT содержит только:

- UserId
- Email

BusinessId в JWT не хранится.

## Consequences

Плюсы

- Stateless API.
- Возможность масштабирования.
- Пользователь может переключаться между компаниями.

Минусы

- Требуется отдельный Refresh Flow.