import React, { useEffect, useRef, useState } from 'react';
import useInterval from 'use-interval';
import * as d3 from 'd3';

// Credit https://wattenberger.com/blog/react-and-d3
const Svg = () => {
  const generateDataset = () => (
    Array(10).fill(0).map(() => ([
      Math.random() * 80 + 10,
      Math.random() * 35 + 10,
    ]))
  );

  const [dataset, setDataset] = useState(generateDataset());

  const ref = useRef();
  useEffect(() => {
    // https://www.d3indepth.com/enterexit/#general-update-pattern
    const updateSelection = d3.select(ref.current)
      .selectAll<SVGCircleElement, number[]>('circle')
      .data(dataset);

    const enterSelection = updateSelection.enter();
    const enterAndUpdateSelection = enterSelection.append('circle').merge(updateSelection);
    enterAndUpdateSelection
      .attr('cx', (d) => d[0])
      .attr('cy', (d) => d[1])
      .attr('r', 3);

    const exitSelection = updateSelection.exit();
    exitSelection.remove();
  }, [dataset]);// Render on time when component loaded

  // https://overreacted.io/making-setinterval-declarative-with-react-hooks/
  useInterval(() => {
    const newDataset = generateDataset();
    setDataset(newDataset);
    console.log('interval');
  }, 1000);

  return <svg viewBox="0 0 100 50" ref={ref} />;
};

export default Svg;
