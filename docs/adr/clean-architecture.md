# ADR-001 Clean Architecture

## Status

Accepted

## Context

Проект должен поддерживать несколько клиентов:
- React
- Telegram
- В будущем Mobile

Необходимо отделить бизнес-логику от инфраструктуры.

## Decision

Используем Clean Architecture.

Слои:

- Core
- Application
- Infrastructure
- API
- Telegram

Core не зависит ни от кого.

Application зависит только от Core.

Infrastructure реализует интерфейсы Application.

API и Telegram являются точками входа.

## Consequences

Плюсы

- Бизнес-логика переиспользуется.
- Telegram и React используют одинаковые Handler'ы.
- Проще тестировать.

Минусы

- Больше проектов.
- Немного больше кода.