[out:csv(::lat,::lon)][timeout:25];
(
  nwr["shop"][!"opening_hours"]({{bbox}});
);
out center;