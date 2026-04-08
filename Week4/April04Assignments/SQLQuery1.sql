CREATE TABLE Employee (
    empid INT PRIMARY KEY,
    empname VARCHAR(50) NOT NULL,
    dept VARCHAR(30),
    salary DECIMAL(10, 2),
    hiredate DATE
);

INSERT INTO Employee (empid, empname, dept, salary, hiredate)
VALUES
(101, 'Nikhil', 'IT', 75000.00, '2026-01-15'),
(102, 'Ravi', 'HR', 68000.50, '2026-03-22'),
(103, 'Boss', 'Finance', 82000.00, '2026-11-05');

Select * from Employee;
