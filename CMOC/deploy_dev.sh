rsync -avz -e 'ssh' bin/Debug/net7.0/ deploy@ec2-54-166-214-229.compute-1.amazonaws.com:/home/deploy/apps/cmoc
ssh deploy@ec2-54-166-214-229.compute-1.amazonaws.com "sudo supervisorctl restart cmoc"