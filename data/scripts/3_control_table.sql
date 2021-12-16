CREATE TABLE public."control" (
	id uuid NOT NULL,	
	delete_normal_message bool NOT NULL,
	delete_dlq_message bool NOT NULL,	
	CONSTRAINT control_pk PRIMARY KEY (id)
);
