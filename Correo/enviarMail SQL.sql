msdb.dbo.sysmail_add_account_sp  
     @account_name =  'IngeniandoGT',
     @email_address =  'ingeniandogt@gmail.com' ,
     @display_name =  'Ingeniando' ,
     @replyto_address =  'ingeniandogt@gmail.com' ,
     @mailserver_name =  'smtp.gmail.com',
     @mailserver_type =  'SMTP' ,
     @port =  587,
     @username =  'ingeniandogt@gmail.com',
     @password =  '@Ingeniandogt/2019',
     @enable_ssl =  TRUE

msdb.dbo.sysmail_add_profile_sp @profile_name = 'IngeniandoGT'

msdb.dbo.sysmail_add_profileaccount_sp
    @profile_name = 'IngeniandoGT' ,
    @account_name = 'IngeniandoGT',
    @sequence_number = 1

EXEC msdb.dbo.sp_send_dbmail @profile_name='IngeniandoGT',
@recipients='jhayr.marin95@gmail.com',
@subject='Recordatorio de Cobro',
@body='Le recordamos realizar su pago.'

SELECT
items.subject,
items.last_mod_date,
l.description
FROM msdb.dbo.sysmail_faileditems as items
INNER JOIN msdb.dbo.sysmail_event_log AS l
ON items.mailitem_id = l.mailitem_id

select*FROM msdb.dbo.sysmail_faileditems

