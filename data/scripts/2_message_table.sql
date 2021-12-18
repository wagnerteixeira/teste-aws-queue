CREATE TABLE public.message (
	id uuid NOT NULL,
	machine_name varchar(100) NOT NULL,
	CONSTRAINT message_pk PRIMARY KEY (id)
);
