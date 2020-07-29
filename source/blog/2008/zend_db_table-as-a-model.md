---
Title: Zend_Db_Table as a Model
Layout: Post
Date: 2008-06-20
Tags: php
---

In the Zend Framework, using Zend_Db_Table as your model class is not well advised. This practice can force you to put business logic in other places besides your model class. This may not be a big deal if your logic is simple but imagine the logic is fairly complicated. Now also imagine that the logic changes at some point. This can be a problem if you have this logic written in many different places.

There's a solution to this problem though. You could write a Model class that uses Zend_Db_Table as a storage medium. Or, you can extend Zend_Db_Table to add your logic to the class. There is a way to simplify this process and that's what this blog post is about.

<!--more-->

For starters, I always like to extend Zend_Db_Table with some intermediate class that I call Db_Table. I do the same for Zend_Db_Table_Row.

```php
class Db_Table extends Zend_Db_Table { ... }
class Db_Table_Row extends Zend_Db_Table_Row { ... }
```

Why do I do this? Say at some later point I figure out that I need some function that the Zend Framework doesn't provide. I just stick that code in Db_Table and now all my Db_Table classes have that functionality. This is actually a good idea for a lot of things in the Zend Framework. Even if you don't add any functionality yourself now, it makes it easier later to add it if you need to.

```php
class Db_Table_Row extends Zend_Db_Table_Row
{
	public function __get($key)
	{
		$inflector = new Zend_Filter_Word_UnderscoreToCamelCase();
		
		$method = 'get' . $inflector->filter($key);
		
		if(method_exists($this, $method))
			return $this->{$method}();
		
		return parent::__get($key);
	}
	
	public function __set($key, $value)
	{
		$inflector = new Zend_Filter_Word_UnderscoreToCamelCase();
		
		$method = 'set' . $inflector->filter($key);
		
		if(method_exists($this, $method))
			return $this->{$method}($value);
			
	        return parent::__set($key, $value);
	}
	
	public function markModified($key)
	{
		$this->_modifiedFields[$key] = true;
	}
}

class Foo extends Db_Table_Row
{
        public function getBar()
        {
                // You must use $this->_data[$key] to access the values in
                // the underlying database. Using $this->bar would cause
                // an infinite loop.
                return $this->_data['bar'] + 5;
        }

        public function setBar($value)
        {
                $this->_data['bar'] = $value - 5;
                
                // This next line tells the underlying Zend_Db_Table class that
                // the value of 'bar' has changed and it should be written to the
                // database.
                $this->markModified('bar');
        }
}

// Now, if foo is a valid instance
echo $foo->bar;
$foo->bar = 5;
```

In this case, when accessing $foo->bar, the method getBar() will be called and the result will be whatever is stored in the database plus 5. Also, when doing $foo->bar = 5, setBar($value) will be called and the number 0 will be stored in the database instead of 5. Granted this is a fairly simple example but I'll explain a better one.

Say you have a date field. At different points in your program you pass in different types of date values. At one point, a timestamp. At another, a string representing a date. At another, an instance of Zend_Date. Using this method, you can store the logic for setting the date that gets stored in one place. It's also helpful if for instance you have to change the underlying field in the database. Maybe you had a Date field at first and now it's become a Timestamp. The point is, the logic is in one place.

This method also more accurately represents what the MVC pattern is supposed to be. There is one place, where the business logic is stored and all the Controller then has to worry about is pushing user input to the Model and requesting output from the Model.
