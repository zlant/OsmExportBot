[out:csv(::lat,::lon)][timeout:25];
(way["building"="apartments"]({{bbox}});)->.builds;
(node["entrance"]({{bbox}});)->.ents;
(way(bn.ents);)->.entbuilds;
(.builds - .entbuilds);
out center;