CREATE TABLE public.message (
	id uuid NOT NULL,
	machine_name varchar(100) NOT NULL,
	process_time timestamp with time zone NOT NULL,
	CONSTRAINT message_pk PRIMARY KEY (id)
);
