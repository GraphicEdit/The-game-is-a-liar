for f in $(git diff --name-only --diff-filter=U | cat); do
   echo "Resolve conflict in $f ..."
   git checkout --theirs $f
done