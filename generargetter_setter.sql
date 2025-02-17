WITH columnas AS (
    SELECT
        c.table_name,
        c.column_name,
        c.data_type,
        c.ordinal_position,
        CASE
            WHEN kcu.column_name IS NOT NULL THEN 'PRIMARY KEY'
            ELSE 'NO'
        END AS key_type
    FROM 
        information_schema.columns c
    LEFT JOIN 
        information_schema.key_column_usage kcu 
        ON c.table_name = kcu.table_name 
        AND c.column_name = kcu.column_name
        AND kcu.constraint_name IN (
            SELECT constraint_name 
            FROM information_schema.table_constraints 
            WHERE constraint_type = 'PRIMARY KEY'
        )
    WHERE 
        c.table_schema = 'public' -- Ajusta si tu esquema es diferente
        AND c.table_name = 'empresa' -- Ajusta aquí el nombre de tu tabla
),
foreign_keys AS (
    SELECT
        kcu.table_name,
        kcu.column_name,
        tc.constraint_name,
        kcu.ordinal_position,
        ccu.table_name AS foreign_table_name,
        ccu.column_name AS foreign_column_name
    FROM 
        information_schema.key_column_usage kcu
    JOIN 
        information_schema.table_constraints tc 
        ON kcu.constraint_name = tc.constraint_name
    JOIN 
        information_schema.constraint_column_usage ccu 
        ON ccu.constraint_name = kcu.constraint_name
    WHERE 
        tc.constraint_type = 'FOREIGN KEY'
        AND kcu.table_name = 'productos' -- Ajusta aquí el nombre de tu tabla
)
SELECT
    'public ' || 
    CASE 
        WHEN columnas.data_type = 'integer' THEN 'int' 
        WHEN columnas.data_type = 'numeric' THEN 'decimal'
        WHEN columnas.data_type LIKE 'character%' THEN 'string'
        WHEN columnas.data_type = 'boolean' THEN 'bool'
        ELSE 'string' -- Ajusta más tipos de datos aquí según sea necesario
    END || ' ' || columnas.column_name || ' { get; set; }' AS csharp_code
FROM 
    columnas
LEFT JOIN 
    foreign_keys fk 
    ON columnas.table_name = fk.table_name 
    AND columnas.column_name = fk.column_name
ORDER BY 
    columnas.table_name, columnas.ordinal_position;
