[out:csv(::lat,::lon)][timeout:25];
(
  nwr["building"][!"addr:housenumber"]({{bbox}});
);
out center;