CREATE TABLE public.errors (
	id uuid NOT NULL,
	machine_name varchar(100) NOT NULL,
	process_time timestamp with time zone NOT NULL,
	message text NOT NULL
);
