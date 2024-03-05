---
title: Api
---

# Api

## `IQuickFormField<T>`

Represents an input field in a QuickForm{TEntity}

### Properties

| Property           | Description                                                            | Notes                                                                 | 
|--------------------|------------------------------------------------------------------------|-----------------------------------------------------------------------|
| EditorId           | Id for the input element.                                              | This is automatically generated                                       |                                   
| DisplayName        | Display name for the input element.                                    |
| Description        | Description for the input element, if any.                             |
| ValidFeedback      | Valid feedback for the input element, if any.                          |
| InputComponent     | Input component template, which is automatically generated.            | The string parameter is the class to be applied to the input element. |
| ValidationMessages | Validation message element template, which is automatically generated. | the string parameter is the class to be applied to the input element. |
