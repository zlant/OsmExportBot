[out:xml][timeout:25];
area({{city}})->.searchArea;
(
  way[~"^parking:lane:.*"~"."](area.searchArea);
);
out body;
>;
out skel qt;