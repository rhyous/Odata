## Entity Relationships

Example: 

```
User
    Id
    UserTypeId
UserType
    Id
    Name

UserRole
    Id
    Name

UserRoleMembership
    Id
    UserId
    UserRoleId
```
    
### User
 - Has a Many-to-One relationship to UserType. Property is in itself, User.
 - Has a One-to-Many relationship to UserRoleMembership. Property is in Foreign Entity, UserRoleMembership.
 - Has a Many-to-Many relationship to UserRole through UserRoleMembership, which holds the properties to both.
      
### UserType
 - Has a One to Many relationship to User. Property is in Foreign Entity, User.

### UserRole
 - Has a One to Many relationship to UserRoleMembership. Property is in Foreign Entity, UserRoleMembership.
 - Has a Many-to-Many relationship to User through UserRoleMembership, which holds the properties to both.