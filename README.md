XNA-Game-and-WPF-Map-Editor
===========================

Simple game with map editor, not complite;

## How it works?!

### First

The game use xml config for building lvl. Why am I not using JSON ?

```bash
DataContractJsonSerializer deserialize = new DataContractJsonSerializer(typeof(object));
MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonInput));
var obj = deserialize.ReadObject(stream) as object;
stream.Close();
```

This code scary me and I use the simple xml parser.

### Second

Game could find all XML files included in solution as lvl. It`s cool. But needs manually including all lvl files.

### Third

I implemented cool Game editor on WPF with MVVM. U could fast and easy build new lvl`s and edit old. 
It supports all objects game have. It could show view of game like it would be on XNA game. 

Sadly you need to rebuild XNA solution to play in.





