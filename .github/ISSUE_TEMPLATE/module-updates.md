---
name: Module Updates
about: Use for specifying new or modified modules
title: ''
labels: ''
assignees: ''

---

### Description
Implement new function and remove deprecated function

### Modification Details
_From module_ `ModuleName`

```
ADD: void Foo(Type param1, Type param2)
DEL: void Bar(Type param1, Type param2, Type param3)
```

### Tests

- [ ] Calling `Foo(2, 3)` should output 5
