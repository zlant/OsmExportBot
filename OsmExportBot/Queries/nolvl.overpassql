[out:csv(::lat,::lon)][timeout:25];
(
  nwr["building"][!"building:levels"]({{bbox}});
);
out center;