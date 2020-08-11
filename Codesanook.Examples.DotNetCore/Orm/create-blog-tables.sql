CREATE TABLE IF NOT EXISTS blogs (
    id INTEGER PRIMARY KEY, 
    -- When you create a table that has an INTEGER PRIMARY KEY column, this column is the alias of the rowid column
    name TEXT NOT NULL,
    description TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS posts (
    id INTEGER PRIMARY KEY, 
    title TEXT NOT NULL,
    details TEXT NOT NULL,
    blog_id INTEGER NOT NULL,
    FOREIGN KEY (blog_id) REFERENCES blogs (id)  -- foreign key to blogs table
);

CREATE TABLE IF NOT EXISTS comments (
    id INTEGER PRIMARY KEY, 
    content TEXT NOT NULL,
    post_id INTEGER NOT NULL,
    FOREIGN KEY (post_id) REFERENCES posts (id)  -- foreign key to posts table
);
