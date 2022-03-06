// Credit https://wattenberger.com/blog/react-and-d3
import React, { useEffect, useRef } from 'react';
import * as d3 from 'd3';
const margin = { left: 50, right: 50, top: 40, bottom: 0 };
const width = 200;
const height = 200;

const dataYears = ['2001', '2002', '2003'];
const dataset = [5, 11, 18];

const y = d3.scaleLinear()
  .domain([0, d3.max(dataset)])
  .range([height, 0]) // invert for y value
const yAxis = d3.axisLeft(y);

const x = d3.scaleBand()
  .domain(dataYears)
  .range([0, width])
  .paddingInner(0.3);

const xAxis = d3.axisBottom(x);

export default function BarChart() {
  const ref = useRef();

  useEffect(() => {
    // https://www.d3indepth.com/v4/enterexit/
    const svg = d3.select(ref.current);
    var chartGroup = svg.append('g').attr('transform', `translate(${margin.left}, ${margin.top})`);

    chartGroup.selectAll('rect')
      .data(dataset)
      .enter().append('rect')
      .attr('x', (d, i) => x(dataYears[i]))
      .attr('y', (d, i) => y(d)) // change y position to get a chart looks rendering as bottom to top
      .attr('height', (d, i) => height - y(d))
      .attr('width', (d, i) => x.bandwidth())
      .attr('fill', 'pink');

    chartGroup.append('g').attr('class', 'axis y').call(yAxis);
    chartGroup.append('g')
      .attr('class', 'axis x hidden')
      .attr('transform', `translate(0, ${height})`)
      .call(xAxis);

  }, []);// Render  when component loaded and dataset changes

  return <svg width='100%' height='100%' ref={ref} />;
};

