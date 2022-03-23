---
name: Module Updates
about: Use for specifying new or modified modules
title: ''
labels: ''
assignees: ''

---

### Description
Player needs to be able to be moved without changing direction to implement things like backoff.

### Public Interface
_From module_ `ModuleName`

```
ADD: void Foo(Type param1, Type param2)
```

### Tests

- [ ] Calling `Foo(2, 3)` should output 5
