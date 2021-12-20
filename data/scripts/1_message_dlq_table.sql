CREATE TABLE public.message_dlq (
	id uuid NOT NULL,
	minute int NOT NULL,
	machine_name varchar(100) NOT NULL,
	process_time timestamp with time zone NOT NULL,
	CONSTRAINT message_dlq_pk PRIMARY KEY (id, minute)
);